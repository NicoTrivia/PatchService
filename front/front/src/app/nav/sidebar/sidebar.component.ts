import {Component, Input, SimpleChanges, OnInit, OnChanges, OnDestroy} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';

// services
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';

@Component({
    selector: 'app-sidebar',
    templateUrl:   './sidebar.component.html',
    styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent extends PatchSecured implements OnInit, OnChanges, OnDestroy {
    @Input() public title: string|null = null;
    public initialized = false;
    private translatedTitle: string|null = null;
    public sidebarSmall = false;
    public selected: string|null = null;
    public displayVersion = false;

    constructor(private readonly translate: TranslateService,
        override readonly authenticationService: AuthenticationService,
        override readonly router: Router) {
        super(authenticationService, router);
    }

    ngOnInit(): void {
        this.initialized = false;
        this.selected = null;
        if (this.title) {
            this.translate.get(this.title).subscribe(msg => {
                this.translatedTitle =  msg;
            });
        }

        if (localStorage.getItem(Config.STORAGE_PS_SIDEBAR_LARGE) === 'false') {
            this.sidebarSmall = true;
        } else {
            this.sidebarSmall = false;
        }
        if (this.selected == null || !this.selected) {
            this.selected = localStorage.getItem(Config.STORAGE_PS_SIDEBAR_ITEM);
        }
        if (this.selected == null || !this.selected || this.selected === 'null') {
            this.selected = 'home';
        }
       
        this.initialized = true;
    }

    ngOnDestroy() {
        // NOOP
    }

    ngOnChanges(changes: SimpleChanges) {
        if ('title' in changes) {
            this.ngOnInit();
        }
    }

    getTitle(): string {
        return this.translatedTitle ? this.translatedTitle : '';
    }
    getVersion(): string {
        return Config.APP_VERSION;
    }
    getUserTitle(): string {
        return 'Bienvenue '+this.getUserName()+' - '+this.getTenant();
    }

    public logout(): void {
        this.router.navigate([`/logout`]);
    }

    public setPassword(): void {
      
    }
       

    public sidebarSize(status: boolean): void {
        this.sidebarSmall = status;
        localStorage.setItem(Config.STORAGE_PS_SIDEBAR_LARGE, (!status).toString());
    }

    public select(dest: string): void {
        this.selected = dest;
        localStorage.setItem(Config.STORAGE_PS_SIDEBAR_ITEM, dest);
        if (dest === 'home') {
            this.router.navigate([`/request_patch`]);
        } else if (dest === 'selectPassword') {
            this.setPassword();
        } 
        else if (dest === 'tawk') {
            window.open("https://dashboard.tawk.to/#/dashboard", "tawk_to");
        } else {
          this.router.navigate([`/`]);
        }

    }

    public isSelected(item: string): boolean {
        return (this.selected === item);
    }

    getUserName(): string {
        if (this.authenticationService.getUser()) {
            return this.authenticationService.getUser()!.firstname + ' '
            + this.authenticationService.getUser()!.lastname;
        }
        return '';
    }

    getTenant(): string {
        if (this.authenticationService.getTenant()) {
            return this.authenticationService.getTenant();
        }
        return '';  
    }

    getProfile(): string {
        return '';  
    }

    public displayVersionDialog() {
        this.displayVersion = true;
    }
}
