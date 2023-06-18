import { Injectable, OnInit} from '@angular/core';
import { Observable, of } from 'rxjs';
import { NGXLogger } from 'ngx-logger';
import { HttpClient, HttpErrorResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import {map, catchError} from 'rxjs/operators';
import {Config} from '../../config';
import * as CryptoJS from 'crypto-js';

import {User} from '../../model/user';
import {Buffer} from 'buffer';

import { PROFILE } from '../profile.enum';
import {PSCommonService} from '../../services/ps-common.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService  extends PSCommonService implements OnInit{

    private user: User|null = null;
    private jwtToken: string|null = null;
    private timeOut = 0;
    private csrfToken: string | null = null;

    private isTimeout = false;
    private initialized = false;

    constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
        super(logger);
        if (!this.initialized) {
            this.ngOnInit();
        }
    }
    public ngOnInit() {
        this.initialized = true;
        this.jwtToken = localStorage.getItem(Config.STORAGE_ACCESS_TOKEN);
        this.timeOut = 0;
        this.clearTimoutDetected();
        if (this.jwtToken) {
            const userStr = localStorage.getItem(Config.STORAGE_USER_OBJ);
            if (userStr)
            {
                const userJson = JSON.parse(userStr);

                this.user = userJson ? new User(userJson) : null;
            }

            this.initTimeOut();
        }
        this.checkExpired();
    }
  
    public getUser(): User|null {
        return this.user;
    }

    public getUserName(): string {
        if (this.getUser()) {
            return this.getUser()!.firstname + ' '
            + this.getUser()!.lastname;
        }
        return '';
    }
    public getTenant(): string {
        return this.getUser() ? this.getUser()!.tenant : '';
    }

    public getJwtToken(): string|null {
        return this.jwtToken;
    }

    public isTimeoutDetected(): boolean
    {
        return this.isTimeout;
    }

    public clearTimoutDetected(): void
    {
        this.isTimeout = false;
    }

    public logout() {
    // remove user from local storage and set current user to null
        localStorage.removeItem(Config.STORAGE_ACCESS_TOKEN);
        localStorage.removeItem(Config.STORAGE_USER_OBJ);
        this.user = null;
        this.jwtToken = null;
        this.timeOut = 0;
    }
  
    /**
     * Ping service to check if alive
     */
    public ping(): Observable<any> {
        return this.http.get(`${Config.APP_URL}${Config.API_ROUTES.ping}`, {observe: 'response'});
    }

    public getCsrfToken(): string {
        return this.csrfToken ? this.csrfToken : '';
    }

    public setCsrfToken(s: string) {
        this.csrfToken = s;
    }
  
    

    public login(tenant: string, login: string, password: string): Observable<User|null> {
        this.isTimeout = false;
        /*const formData: FormData = new FormData();
        formData.append('tenant', tenant);
        formData.append('login', login);
        formData.append('password', password);
        return this.http.post<any>(`${Config.APP_URL}${Config.API_ROUTES.authenticate}`, formData)
            .pipe(map(user => {
                return this.setUser(user);
            }),
            catchError(err => {
                this.handleError(err);
                return of(null); }));
                */
        if (tenant === 'ACME' && 'nicolas' === login && 'nicolas' == password) {
            const user = new User('');
            user.login = login;
            user.firstname = "Nicolas";
            user.lastname = "Duval";
            user.tenant = tenant;
            user.email = "nicolas.duval@acme.fr"
            user.profile = PROFILE.CUSTOMER;
            user.active = true;
            this.setUser(user);
            return of(user);
        }
        if (tenant === 'AutoBis' && 'julia' === login && 'julia' == password) {
            const user = new User('');
            user.login = login;
            user.firstname = "Julia";
            user.lastname = "Valasky";
            user.tenant = tenant;
            user.email = "j.valasky@autobis.com"
            user.profile = PROFILE.CUSTOMER;
            user.active = true;
            this.setUser(user);

            return of(user);
        }
        if (tenant === 'AutoBis' && 'frank' === login && 'frank' == password) {
            const user = new User('');
            user.login = login;
            user.firstname = "Frank";
            user.lastname = "Sably";
            user.tenant = tenant;
            user.profile = PROFILE.CUSTOMER;
            user.email = "f.sably@autobis.com"
            user.active = false;
            this.setUser(user);

            return of(user);
        }
        if (tenant === 'EA-Tech' && 'lionel' === login && 'lionel' == password) {
            const user = new User('');
            user.login = login;
            user.firstname = "Lionel";
            user.lastname = "Gros";
            user.email = "support@eatech.com"
            user.tenant = tenant;
            user.profile = PROFILE.OPERATOR;
            user.active = true;
            this.setUser(user);

            return of(user);
        }
        return of(null);    
    }

    private setUser(user :any): User|null
    {
    // store user details and jwt token in local storage to keep user logged in between page refreshes
        if (!user) {
            return null;
        }
        //this.logger.warn('userObj %o : %o', userObj, user);
        // JWT token OR API Key
        /*const userObj = new User(user);
        this.jwtToken = userObj.jwtToken;
        */
        const userObj = user;
        this.jwtToken = user.login;
        try {
            localStorage.setItem(Config.STORAGE_ACCESS_TOKEN, this.jwtToken ? this.jwtToken: '');
        } catch (err1) {
            localStorage.clear();
            localStorage.setItem(Config.STORAGE_ACCESS_TOKEN, this.jwtToken ? this.jwtToken: '');
        }
        userObj.clearJwtToken();
        this.updateUser(userObj);
        this.initTimeOut();
        return this.user;
    }
  
    private initTimeOut() {
        this.timeOut = new Date().getTime();
    }

    public updateUser(user: User) {
        this.user = user;
        if (this.user)
        {
            localStorage.setItem(Config.STORAGE_USER_OBJ, JSON.stringify(this.user));
        }
        localStorage.setItem(Config.STORAGE_LOGIN, this.getLogin() ? this.getLogin() : '');
    }

    public getLogin(): string {
        return this.getUser() != null ? this.getUser()!.login : '';
    }

    public encryptData(data: string): string|null {
        try {
            return CryptoJS.AES.encrypt(JSON.stringify(data), this.getTenant()).toString();
        } catch (e) {
            this.logger.warn(e);
        }
        return null;
    }

    /**
     * check wether session is already timeout or near timemout (then reset token if reinit is true)
     * @param reinit allow renew a session near timeout for inactivity
     * @returns 
     */
    public checkTimeOut(reinit: boolean): boolean {
        const time = new Date().getTime();
        if (!this.timeOut || this.timeOut <= 0) {
            return false;
        }
        this.checkExpired();
        if (this.timeOut < (time - Config.TIMEOUT_CHECK)) {
            // renew JWT
            this.http.get<string>(`${Config.APP_URL}${Config.API_ROUTES.reset_timeout}`).pipe(map(tokenStr => {
                this.initTimeOut();
                if (tokenStr === null || tokenStr === '{}') {
                    return null;
                }
                const jsonStr = JSON.stringify(tokenStr);
                const newToken = JSON.parse(jsonStr);
                return newToken.access_token;
            })).subscribe(accessToken => {
                if (reinit && accessToken) {
                    this.jwtToken = accessToken;
                    localStorage.setItem(Config.STORAGE_ACCESS_TOKEN, this.jwtToken!);
                }
            });
        }
        return true;
    }

    /**
     * Check on backend if Jwt token still active
     */
    public detectTimeOut(): void {
        this.http.get<string>(`${Config.APP_URL}${Config.API_ROUTES.check_session}`).pipe(map(statusStr => {

            if (statusStr === null) {
                this.isTimeout = true;
            } else {
                const statusJson = JSON.parse(JSON.stringify(statusStr));
                if (statusJson && !statusJson.status)
                {
                    this.isTimeout = true;
                }
            }

        })).subscribe(t => {
            // NO OP
        });
    }

    public validateToken(tenant: string, tokenId: string): Observable<User|null> {
        this.isTimeout = false;
        const formData: FormData = new FormData();
        formData.append('tenant', tenant);
        formData.append('tokenId', tokenId);
        return this.http.post<any>(`${Config.APP_URL}${Config.API_ROUTES.validate_token}`, formData)
            .pipe(map(user => {
                return this.setUser(user);
            }),
            catchError(err => {
                this.handleError(err);
                return of(null); }));
    }

    /**
     * 
     * @returns check if jwt token is not expired
     */
    private checkExpired(): boolean {
        /*if (this.jwtToken) {
            const payloadBase64 = this.jwtToken.split('.')[1];
            const decodedJson = Buffer.from(payloadBase64, 'base64').toString();
            const decoded = JSON.parse(decodedJson);
            const exp = decoded.exp;
            const expired = (Date.now() >= exp * 1000);
            let ok = true;
            if (expired) {
                this.logger.warn('JWT token has expired');
                ok = false;
            } if (decoded.iss != 'SLFP') {
                this.logger.warn('JWT token not valid');
                ok = false;
            } else if (this.getTenant() != null && this.getTenant() != decoded.tenant) {
                this.logger.warn('JWT has invalid tenant %s : %s', this.getTenant(), decoded.tenant);
                ok = false;
            } else if (this.getLogin() != null && this.getLogin() != decoded.user_login) {
                this.logger.warn('JWT has invalid login %s : %s', this.getLogin(), decoded.user_login);
                ok = false;
            }
            if (!ok) {
                this.jwtToken = null;
                this.user = null;
            }
        }*/
        return this.jwtToken != null;
    }

    public allow(p: PROFILE): boolean {
        if (!this.user || !this.jwtToken || this.isTimeout) {
            return false;
        }
        return this.user.allow(p);
    }
    public encodeWord(w: string) {
        return '*' + encodeURIComponent(btoa(w)) + '*';
    }

    public encodeTenantUrl(url: string, lang: string) {
        if (url
            && this.getTenant() != null
            && this.getTenant().length > 1) {
            const key = '/' + this.encodeWord(this.getTenant());
            if (!url.includes(key)) {
                url = url + key;
            }
        }

        if (url
            && this.getJwtToken() != null
            && this.getJwtToken()!.length > 1) {
            const key = '?token_type=Bearer&access_token=' + encodeURIComponent(this.getJwtToken()!);
            if (!url.includes(key)) {
                url = url + key;
            }
        }
        if (lang)
        {
            if (url.includes('?')) {
                url = url+'&lang='+lang;
            } else {
                url = url+'?lang='+lang;
            }
        }
        return url;
    }
}
