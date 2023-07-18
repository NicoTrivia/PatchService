import {Router} from '@angular/router';
import { PROFILE } from './profile.enum';

// services
import { AuthenticationService } from '../auth/authentication-service/authentication-service';

export class PatchSecured {
    public PROFILE = PROFILE;
    constructor(readonly authenticationService: AuthenticationService, readonly router: Router) {
        if (!this.authenticationService.checkTimeOut(true)) {
            this.router.navigate([`/logout`]);
        }
    }

    public allow(p: PROFILE): boolean {
        return  this.authenticationService.allow(p);
    }
}
