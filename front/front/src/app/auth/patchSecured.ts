import {Router} from '@angular/router';
import { PROFILE } from './profile.enum';

// services
import { AuthenticationService } from '../auth/authentication-service/authentication-service';

export class PatchSecured {
    public PROFILE = PROFILE;
    constructor(readonly authenticationService: AuthenticationService, readonly router: Router) {
        this.authenticationService.checkTimeOut(true);
    }

    public allow(profile: PROFILE): boolean {
       
        return true;
    }
}
