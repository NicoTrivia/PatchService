import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RequestPatchComponent} from './nav/request-patch/request-patch.component';
import {UserListComponent} from './nav/user-list/user-list.component';
import {EditUserComponent} from './forms/edit-user/edit-user.component';
import {BrandListComponent} from './nav/brand-list/brand-list.component';
import {TenantListComponent} from './nav/tenant-list/tenant-list.component';
import {TicketListComponent} from './nav/ticket-list/ticket-list.component';
import {TicketInProgressComponent} from './nav/ticket-in-progress/ticket-in-progress.component';
import {EditTenantComponent} from './forms/edit-tenant/edit-tenant.component';
import {EditBrandComponent} from './forms/edit-brand/edit-brand.component';
import {EditEcuComponent} from './forms/edit-ecu/edit-ecu.component';
import {EditMailComponent} from './forms/edit-mail/edit-mail.component';

import {AuthGuard} from './auth/auth-guard/authGuard';
import {LoginComponent} from './auth/login/login.component';
import {LogoutComponent} from './auth/logout/logout.component';

const routes: Routes = [
  { path: '', component: RequestPatchComponent, canActivate: [AuthGuard] } ,
  
  { path: 'request_patch', component: RequestPatchComponent, canActivate: [AuthGuard]  },
  { path: 'user_list', component: UserListComponent, canActivate: [AuthGuard]  },
  { path: 'edit_brand', component: EditBrandComponent, canActivate: [AuthGuard]  },
  { path: 'edit_brand/:code', component: EditBrandComponent, canActivate: [AuthGuard]  },
  { path: 'edit_mail_template', component: EditMailComponent, canActivate: [AuthGuard]  },
  { path: 'edit_ecu/:brand_code', component: EditEcuComponent, canActivate: [AuthGuard]  },
  { path: 'edit_ecu/:brand_code/:code', component: EditEcuComponent, canActivate: [AuthGuard]  },
  { path: 'edit_user', component: EditUserComponent, canActivate: [AuthGuard]  },
  { path: 'edit_user/:id', component: EditUserComponent, canActivate: [AuthGuard]  },
  { path: 'edit_user/:password/:id', component: EditUserComponent, canActivate: [AuthGuard]  },
  
  { path: 'tenant_list', component: TenantListComponent, canActivate: [AuthGuard]  },
  { path: 'brand_list', component: BrandListComponent, canActivate: [AuthGuard]  },
  { path: 'edit_tenant', component: EditTenantComponent, canActivate: [AuthGuard]  },
  { path: 'edit_tenant/:code', component: EditTenantComponent, canActivate: [AuthGuard]  },
  { path: 'ticket', component: TicketListComponent, canActivate: [AuthGuard]  },
  { path: 'ticket_in_progress', component: TicketInProgressComponent, canActivate: [AuthGuard]  },
  
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
