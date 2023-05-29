import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

// services
import { AuthenticationService } from '../authentication-service/authentication-service';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit {
    constructor(private readonly route: ActivatedRoute,
        private readonly authenticationService: AuthenticationService,
        private readonly router: Router) {
    }

    ngOnInit() {
        this.authenticationService.logout();
        this.router.navigate([`/login`]);
    }
}
