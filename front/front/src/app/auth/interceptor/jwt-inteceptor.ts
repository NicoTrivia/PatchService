import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse
} from '@angular/common/http';

import { Observable, throwError, of } from 'rxjs';
import {AuthenticationService} from '../authentication-service/authentication-service';
import {catchError, } from 'rxjs/operators';

/** Pass untouched request through to the next request handler. */
@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private readonly authenticationService: AuthenticationService) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
        // Clone the request and replace the original headers with
        // cloned headers, updated with the authorization.
        const tenant = this.authenticationService.getTenant();
        const jwkToken = this.authenticationService.getJwtToken();
        const userLogin = this.authenticationService.encryptData(this.authenticationService.getLogin());
        if (req.url.endsWith('/ping') || req.url.endsWith('/logout') || req.url.endsWith('/validate_token')) {
            // do nothing
        }
        else if (req.method === 'POST' && req.url.endsWith('/authenticate')) {
            let newHeaders = req.headers;

            newHeaders = newHeaders.set('X-CSRF-TOKEN', this.authenticationService.getCsrfToken());

            const authReq = req.clone({
                headers: newHeaders
            });

            // send cloned request with header to the next handler.
            return next.handle(authReq).pipe(
                catchError((error) => this.handleError(error, this.authenticationService)));
        }
        else if ((tenant != null && tenant.length > 1) || (jwkToken != null && jwkToken.length > 1)) {
            let newHeaders = req.headers;
            if (tenant != null && tenant.length > 1) {
                newHeaders = newHeaders.set('x-consumer-id', tenant);
            }

            if (userLogin != null && userLogin.length > 1) {
                newHeaders = newHeaders.set('x-consumer-username', userLogin);
            }
            if (jwkToken != null && jwkToken.length > 1) {
                newHeaders = newHeaders.set('Authorization', 'Bearer ' + jwkToken);
            }

            const authReq = req.clone({
                headers: newHeaders
            });

            // send cloned request with header to the next handler.
            return next.handle(authReq).pipe(
                catchError((error) => this.handleError(error, this.authenticationService)));
        } 
        return next.handle(req).pipe(
            catchError((error) => this.handleError(error, this.authenticationService)));
    }

    private handleError(error: HttpErrorResponse, authenticationService: AuthenticationService) {
        if (error.status === 401 && authenticationService) {
            authenticationService.logout();
        }
        return throwError(error);
    }
}
