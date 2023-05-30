import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MessageService } from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';
import {Brand} from '../../model/brand';
// services
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TawkService } from '../../services/TawkService';
@Component({
  selector: 'app-request-patch',
  templateUrl: './request-patch.component.html',
  styleUrls: ['./request-patch.component.css'],
  providers: [MessageService]
})


export class RequestPatchComponent extends PatchSecured implements OnInit {
  
  protected fileName: string|null = null;
  protected fileSize: number = 0;
  protected brandList: Brand[] = [];
  protected brandSelected: Brand|null = null;

  protected deviceList: Brand[] = [];
  protected deviceSelected: Brand|null = null;

  protected placeholderBrand: string = ' ';
  protected placeholderDevice: string = ' ';

  protected pr_dpf = false;
  protected pr_egr = false;
  protected pr_tva = false;
  protected pr_start_stop = false;
  protected pr_adblue = false;
  protected pr_lambda = false;
  protected pr_readiness = false;
  protected pr_maf = false;
  protected pr_flaps = false;

  constructor(private readonly translate: TranslateService, private messageService: MessageService, 
    override readonly authenticationService: AuthenticationService,
    override readonly router: Router,
    private TawkService: TawkService) {
    super(authenticationService, router);
  }

  
  ngOnInit() {
    this.translate.get("REQUEST_PATCH.MSG.SELECT_BRAND").subscribe(msg => {
      this.placeholderBrand = msg;
    });
    this.translate.get("REQUEST_PATCH.MSG.SELECT_DEVICE").subscribe(msg => {
      this.placeholderDevice = msg;
    });
    this.brandList = [
      { name: '', code: '' },
      { name: 'Alpha', code: 'ALPHA' },
      { name: 'BMW', code: 'IST' },
      { name: 'Fiat', code: 'FIAT' },
      { name: 'Lancia', code: 'LANCIA' },
      { name: 'Volkswagen VAG', code: 'VAG' },
      { name: 'Peugeot', code: 'PEUGEOT' }

  ];

  }

  onBasicUploadAuto(event: any) {
    if (event.files && event.files[0])
    {
      //console.log("event %o",event.files[0]);
      this.fileName = event.files[0].name;
      this.fileSize = event.files[0].size;
      if (this.fileSize > 0) {
        this.fileSize = Math.trunc(this.fileSize / 1024);
      }
      
      this.translate.get("REQUEST_PATCH.MSG.UPLOAD").subscribe(msg => {
        this.messageService.add({ severity: 'info', summary: 'Information', detail: msg });
      });
    }
  }

  updateDevices():void {
    this.deviceSelected = null;
    this.deviceList= [];
    if (this.brandSelected && this.brandSelected.code.length > 0) {
      for (var _i = 0; _i < 2 * this.brandSelected.code.length; _i++) {
        this.deviceList.push({ code: 'EQ_'+this.brandSelected.code+'_'+_i, name: 'Calc. '+this.brandSelected.name+" n° "+_i});
      }
    }
  }

  canSubmit() : boolean {
    if (this.fileName 
        && this.brandSelected && this.brandSelected.code != '' 
        && this.deviceSelected && this.deviceSelected.code != ''
        && (this.pr_dpf || this.pr_egr || this.pr_tva || this.pr_start_stop
          || this.pr_adblue || this.pr_lambda || this.pr_readiness|| this.pr_maf|| this.pr_flaps)) {
      return true;
    }
    return false;
  }
  submitTicket() {
    this.messageService.add({ severity: 'success', summary: 'Information', detail: "Une fenetre de confirmation s''affichera ici." });
  }
}
