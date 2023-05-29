import {HttpErrorResponse } from '@angular/common/http';
import { NGXLogger } from 'ngx-logger';
import {throwError} from 'rxjs';

export class PSCommonService {
    protected firstCall = true;
    protected accessDenied = false;
    private lastErrorStatus= 0;

    constructor( protected readonly logger: NGXLogger) {
        this.init();
    }

    protected init(): void {
        this.firstCall = true;
        this.accessDenied = false;
    }

    protected handleError(error: HttpErrorResponse) {
        this.lastErrorStatus = error.status;
        this.logger.warn(error);

        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            this.logger.error('An error occurred:', error.error.message);
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            this.logger.error(
                `Backend returned code ${error.status}, ` +
            `body was: ${error.message}`);
            if (/*error.status === 0 /*CORS || */error.status === 401 || error.status === 403) {
                this.accessDenied = true;
            }
        }

        // return an observable with a user-facing error message
        return throwError('Unexpected error');
    }

    public getLastErrorStatus(): number {
        return this.lastErrorStatus;
    }
}
