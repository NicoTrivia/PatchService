import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';
import {Brand} from '../../model/brand';
import {Ecu} from '../../model/ecu';
import {Ticket} from '../../model/ticket';
import {ItemInterface} from '../../model/ItemInterface';

// services
import { TawkService } from '../../services/TawkService';

import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { BrandService } from '../../services/brand.service';
import { EcuService } from '../../services/ecu.service';

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

  protected deviceList: any[] = [];
  protected deviceSelected: any|null = null;

  protected placeholderBrand: string = ' ';
  protected placeholderDevice: string = ' ';
  protected immatriculation: string = '';
  protected fuelList: ItemInterface[] = [];
  protected fuelSelected: ItemInterface | null = null;
  
  protected ecu_sel: Ecu = new Ecu('');

  protected ticket: Ticket| null = null;
  protected ticketConfirmVisible = false;
  
  constructor(private readonly translate: TranslateService, private messageService: MessageService, 
    override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly brandService: BrandService,
    private readonly ecuService: EcuService,
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
    this.brandList = [];

    this.brandService.findAll().subscribe(br => {
      this.brandList = br;
    });

    this.fuelList =  [
      { name: 'Toutes', code: 'A' },
      { name: 'Diesel', code: 'D' },
      { name: 'Essence', code: 'P' }
    ];
    this.fuelSelected = this.fuelList[0];
    if (Config.APP_URL.includes('5000')) {
      this.fileName="dev_test.zip"
    }
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
      this.ecuService.findByBrand(this.brandSelected.code).subscribe(ecu => {
        this.deviceList = ecu;
      });
    }
  }

  canSubmit() : boolean {
    if (this.fileName 
        && this.brandSelected && this.brandSelected.code != '' 
        && this.deviceSelected && this.deviceSelected.code != '') {
      return true;
    }
    return false;
  }
  submitTicket() {
    this.ticket = new Ticket();
    this.ticket.updateFromEcu(this.ecu_sel);
    this.ticket.tenant = this.authenticationService.getTenant();
    this.ticket.customer_level = "Silver";
    this.ticket.user_name = this.authenticationService.getUserName();
    this.ticket.date = new Date();
    this.ticket.filename = this.fileName!;
    this.ticket.file_size = this.fileSize;
    this.ticket.immatriculation = this.immatriculation;
    this.ticket.fuel = this.fuelSelected!.name;
   
    this.ticket.brand_code = this.brandSelected!.code;
    this.ticket.brand_name = this.brandSelected!.name;
    this.ticket.ecu_code = this.deviceSelected!.code;
    
    this.ticketConfirmVisible = true;
  }

  confirmTicketEvent(resu: string) {
    this.ticketConfirmVisible = false;
  }
}
