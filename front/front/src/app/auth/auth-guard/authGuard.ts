import {inject} from '@angular/core';
import { Router} from '@angular/router';
import { AuthenticationService } from '../authentication-service/authentication-service';

export const AuthGuard = () => {
    const router = inject(Router);
    const service = inject(AuthenticationService);
    if (service.getUser() && service.getJwtToken() != null && service.getUser()!.active) {
        // authorised so return true
        return true;
    }

    // not logged in so redirect to login page with the return url
    router.navigate(['/login']);
    return false;
};
