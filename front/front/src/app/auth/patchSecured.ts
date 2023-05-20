import {Router} from '@angular/router';
import { PROFILE } from './profile.enum';

export class PatchSecured {
    public PROFILE = PROFILE;
    constructor(readonly router: Router) {
    }

    public allow(profile: PROFILE): boolean {
       
        return true;
    }
}
