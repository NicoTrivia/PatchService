import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RequestPatchComponent} from './nav/request-patch/request-patch.component';
import {AuthGuard} from './auth/auth-guard/authGuard';
import {LoginComponent} from './auth/login/login.component';
import {LogoutComponent} from './auth/logout/logout.component';

const routes: Routes = [
  { path: '', component: RequestPatchComponent, canActivate: [AuthGuard] } ,
  { path: 'request_patch', component: RequestPatchComponent, canActivate: [AuthGuard]  },
  { path: 'login', component: LoginComponent  },
  { path: 'logout', component: LogoutComponent  },

  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,  { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
