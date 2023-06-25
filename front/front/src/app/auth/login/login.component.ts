import {Component, OnInit} from '@angular/core';
import { NGXLogger } from 'ngx-logger';
import {ActivatedRoute, Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import {Config} from '../../config';

import { AuthenticationService } from '../../auth/authentication-service/authentication-service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    private initialized = false;
    private pingOk = false;
    private pingTime= 0;
    private tenant: string| null = null;
    private login: string| null = null;
    private password: string| null = null;
    private countCheckConnection= 0;

    constructor(private readonly route: ActivatedRoute, private readonly logger: NGXLogger,
        private readonly translate: TranslateService,
        private readonly authenticationService: AuthenticationService,
        private readonly router: Router, private messageService: MessageService,
        private readonly cookieService: CookieService) {
    }
    ngOnInit() {
        this.initialized = false;
        this.countCheckConnection = 0;
        this.pingOk = false;
 
        this.tenant = localStorage.getItem(Config.STORAGE_TENANT);
        this.login = localStorage.getItem(Config.STORAGE_LOGIN);

        this.checkConnection();
        setTimeout(() => this.initialized = true, 4000);
    }
    ping(): boolean {
        return this.pingOk;
    }
    isInitialized(): boolean {
        return this.initialized;
    }

    public checkConnection(): void {
      this.pingOk = true;
      /*
        this.authenticationService.clearTimoutDetected();
        if (this.pingOk && (Date.now() - this.pingTime) < 50000) {
        // this.logger.degug('Cnx check Ignore diff : ' + (Date.now() - this.pingTime));

            return; // connexion OK
        }
        if (this.authenticationService.getJwtToken()) {
            this.authenticationService.logout(); // clear all bearer info
        }
        //        this.logger.degug('Cnx check performed : diff : ' + (Date.now() - this.pingTime));
        this.authenticationService.ping().subscribe((resp) => {
            if (resp) {
                this.pingOk = true;
                this.pingTime = Date.now();
                let csrfToken = this.cookieService.get('XSRF-TOKEN');
                if (!csrfToken && resp.body.session_id && resp.body.key) {
                    csrfToken = resp.body.session_id.substring(2) + resp.body.key.substring(2);
                }
                if (csrfToken) {
                    this.authenticationService.setCsrfToken(csrfToken);
                }
            }
        });
        if (this.countCheckConnection < 5) {
            this.countCheckConnection++;
            setTimeout(() => this.checkConnection(), 6000);
        }
        */
    }

    /**
     * Validation of form : save parameters
     */
    public validateForm(): void {
        if (!this.login || !this.password || !this.tenant) {
            this.translate.get('WARNING.MISSING_VALUE').subscribe(msg => {
              this.messageService.add({ severity: 'warn', summary: 'Information', detail: msg });
            });
            return;
        }
        
        localStorage.setItem(Config.STORAGE_TENANT, this.tenant == null ? '' : this.tenant);
        localStorage.setItem(Config.STORAGE_LOGIN, this.login);

        this.authenticationService.login(this.tenant, this.login, this.password).subscribe((user) => {
            if (user) {
                if (!user.active) {
                    this.password = '';
                    this.translate.get('LOGIN_FORM.MSG.UNACTIVE').subscribe(msg => {
                      this.messageService.add({ severity: 'warn', summary: 'Information', detail: msg })
                    });
                } else {
                    setTimeout(() => this.router.navigate(['/request_patch']), 400);
                }
            } else {
                this.password = '';
                this.translate.get('LOGIN_FORM.MSG.FAILED').subscribe(msg => {
                  this.messageService.add({ severity: 'warn', summary: 'Information', detail: msg })
                });
            }
        });
    }

    set Tenant(n: string|null) {
        this.tenant = n;
    }

    get Tenant(): string|null {
        return this.tenant;
    }

    set Login(n: string|null) {
        this.login = n;
    }

    get Login(): string|null {
        return this.login;
    }

    set Password(n: string|null) {
        this.password = n;
        this.checkConnection();
    }

    get Password(): string|null{
        return this.password;
    }

    getVersion(): string {
        return Config.APP_VERSION;
    }
}
