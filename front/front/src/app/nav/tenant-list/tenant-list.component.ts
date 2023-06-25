import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { MessageService } from 'primeng/api';

import {Tenant} from '../../model/tenant';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TenantService } from '../../services/tenant.service';

@Component({
  selector: 'app-tenant-list',
  templateUrl: './tenant-list.component.html',
  styleUrls: ['./tenant-list.component.css']
})
export class TenantListComponent extends PatchSecured implements OnInit {
  
  public tenantList:Tenant[] = [];

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly tenantService: TenantService,
    private readonly translate: TranslateService, private messageService: MessageService
    ) {
    super(authenticationService, router);
  }
  
  ngOnInit() {
    this.reload();
  }
  reload() {
    // load list from server
    this.tenantService.findAll().subscribe(list => {
      this.tenantList = list;
    });
  }

  public activate(tenant: Tenant, status: boolean): void {
    if (this.authenticationService.getTenant() === tenant.code) {
        this.translate.get('WARNING.NOT_SELF').subscribe(msg => {
            this.messageService.add({ severity: 'warn', summary: 'Information', detail: msg });
        });
        return;
    }

    tenant.active = status;
    this.tenantService.set(tenant).subscribe(s =>  this.reload());
          this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
            this.messageService.add({ severity: 'success', summary: 'Information', detail: msg });
          });

  }
  
  public set(code: string): void {
    this.router.navigate([`/tenant/${code}`]);
  }
}
