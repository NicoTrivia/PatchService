import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { ConfirmationService, MessageService, ConfirmEventType } from 'primeng/api';
import {Brand} from '../../model/brand';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { BrandService } from '../../services/brand.service';
@Component({
  selector: 'app-brand-list',
  templateUrl: './brand-list.component.html',
  styleUrls: ['./brand-list.component.css']
})
export class BrandListComponent  extends PatchSecured implements OnInit {
  public brandList:Brand[] = [];

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly brandService: BrandService,
    private readonly translate: TranslateService, private messageService: MessageService,
    private confirmationService: ConfirmationService
    ) {
    super(authenticationService, router);
  }
  
  ngOnInit() {
    this.reload();
  }

  reload() {
    // load list from server
    this.brandService.findAll().subscribe(list => {
      this.brandList = list;
    });
  }

  public set(brand: Brand): void {
    this.router.navigate([`/edit_brand/${brand.code}`]);
  }

  public cancel(): void {
    this.router.navigate(['/user_list']);
  }

  delete(brand: Brand) {

    this.confirmationService.confirm({
      message: 'Confirmez-vous la suppression de la marque '+brand.name+' (code: '+brand.code+') ? Cette opération ne peut pas être annulée.',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.brandService.delete(brand.code).subscribe(s =>  this.reload());
        this.translate.get('WARNING.DATA_DELETED').subscribe(msg => {
          this.messageService.add({ severity: 'info', summary: 'Information', detail: msg });
        });
        },
      reject: () => {
       
      }
  });
  }
}
