import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import {map} from 'rxjs/operators';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';
import {Brand} from '../../model/brand';
import {Ecu} from '../../model/ecu';
import {Ticket} from '../../model/ticket';
import {ItemInterface} from '../../model/ItemInterface';
import {User} from '../../model/user';

// services
import { TawkService } from '../../services/TawkService';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { BrandService } from '../../services/brand.service';
import { EcuService } from '../../services/ecu.service';
import { TicketService } from '../../services/ticket.service';
import { TenantService } from '../../services/tenant.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-request-patch',
  templateUrl: './request-patch.component.html',
  styleUrls: ['./request-patch.component.css']  
})


export class RequestPatchComponent extends PatchSecured implements OnInit {
  
  protected fileName: string|null = null;
  protected fileSize: number = 0;
  protected file_id: string|null = null;
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
  protected nextFileId: number = 1;

  constructor(private readonly translate: TranslateService, private messageService: MessageService, 
    override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly brandService: BrandService,
    private readonly ecuService: EcuService, private readonly ticketService: TicketService,
    private TawkService: TawkService, private readonly tenantService: TenantService) {
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
    this.getNextFileId(true);
  }

  onBasicUploadAuto(event: any) {
    this.file_id = ''+this.nextFileId;
    this.getNextFileId(false);
    if (event.files && event.files[0])
    {
      console.log("event %o",event.files[0]);
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
    this.tenantService.findByCode(this.authenticationService.getTenant()).subscribe(tenant => {

      const user: User|null = this.authenticationService.getUser();

      this.ticket = new Ticket();
      this.ticket.updateFromEcu(this.ecu_sel);
      this.ticket.tenant = tenant != null ? tenant.code : this.authenticationService.getTenant();
      this.ticket.customer_level =  tenant ? tenant.level : "Silver";
      this.ticket.user_id = user == null ? -1 : user.id;
      this.ticket.user_name = user == null ? '' : user.firstname + ' ' + user.lastname;
      this.ticket.date = new Date();
      this.ticket.filename = this.fileName!;
      this.ticket.file_id = this.file_id;
      this.ticket.file_size = this.fileSize;
      this.ticket.immatriculation = this.immatriculation;
      this.ticket.fuel = this.fuelSelected!.name;
     
      this.ticket.brand_code = this.brandSelected!.code;
      this.ticket.brand_name = this.brandSelected!.name;
      this.ticket.ecu_code = this.deviceSelected!.code;
      
      this.ticketConfirmVisible = true;   
    });
  }

  confirmTicketEvent(resu: string) {
    this.ticketConfirmVisible = false;
  
    if (resu === 'true' && this.ticket) {
      console.log("1 CREATE TICKET");
      this.ticketService.create(this.ticket).subscribe(t => {
        console.log("2 CREATE TICKET %o", t);
        if (t) {
          this.ticket = t;
          this.translate.get("REQUEST_PATCH.MSG.CREATED", {'id': this.ticket.id}).subscribe(msg => {
            this.messageService.add({ severity: 'success', summary: 'Ticket soumis', detail: msg });
          });
          console.log("3 CREATE TICKET %o", t);

          this.router.navigate([`/ticket`]);
        }
      });
    }
  }

  getNextFileId(refresh: boolean) {
    this.tenantService.getNextFileId().subscribe(id => {
      this.nextFileId = id;
      if (refresh) {
          setTimeout(() => {this.getNextFileId(refresh);}, 10000);
      }
    });
  }

  getUploadUrl(): string {
    const id = this.nextFileId;
    const url = `${Config.APP_URL}${Config.API_ROUTES.files}/${id}`;
    return url;
  }
}
