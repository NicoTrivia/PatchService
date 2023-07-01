import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TenantService } from '../../services/tenant.service';
import { MessageService } from 'primeng/api';
import {Config} from '../../config';

import {Tenant} from '../../model/tenant';

@Component({
  selector: 'app-edit-tenant',
  templateUrl: './edit-tenant.component.html',
  styleUrls: ['./edit-tenant.component.css']
})
export class EditTenantComponent extends PatchSecured  implements OnInit {

  tenant: Tenant|null = null;
  isCreation = true;

  constructor(private readonly route: ActivatedRoute, private readonly tenantService: TenantService,
      private readonly translate: TranslateService,
      override readonly router: Router,
      private messageService: MessageService,
      override readonly authenticationService: AuthenticationService) {
      super(authenticationService, router);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
        const cId:string = params['code'];
       
        if (!cId) {
            // creation form
            this.tenant = new Tenant('');
            this.tenant.code = '';
            this.tenant.active = true;
        } else {
            // set form : get value
            this.tenantService.findByCode(cId).subscribe((t) => {
                if (t) {
                    this.tenant = t;
                    this.isCreation = false;
                } else {
                  this.tenant = new Tenant('');
                  this.tenant.code = '';
                  this.tenant.active = true;
                }
            });
        }
    });
  }

  
  public cancelForm(): void {
    this.router.navigate(['/tenant_list']);
  }
 
  /**
    * Validation of form : save
    */
  public validateForm(): void {
    if (!this.tenant || !this.tenant.code || !this.tenant.name) {
        this.translate.get('WARNING.NO_VALUE').subscribe(msg => {
          this.messageService.add({ severity: 'warn', summary: 'Attention', detail: msg });
      });
      return;
    }
    
    if (this.isCreation)
      this.tenantService.create(this.tenant).subscribe(r => this.success(r));
    else
      this.tenantService.set(this.tenant).subscribe(r => this.success(r));

  }
 
  success(f: Tenant): void {
    this.cancelForm(); // check where we go now
    this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
      this.messageService.add({ severity: 'info', summary: 'Informarion', detail: msg })
    });
}
}
