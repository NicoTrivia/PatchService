import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { EcuService } from '../../services/ecu.service';
import { BrandService } from '../../services/brand.service';
import { ConfirmationService, MessageService, ConfirmEventType } from 'primeng/api';
import {Config} from '../../config';

import {Brand} from '../../model/brand';
import {Ecu} from '../../model/ecu';

@Component({
  selector: 'app-edit-brand',
  templateUrl: './edit-brand.component.html',
  styleUrls: ['./edit-brand.component.css']
})
export class EditBrandComponent extends PatchSecured  implements OnInit {

  brand: Brand|null = null;
  isCreation = true;
  protected ecuList: Ecu[] = [];

  constructor(private readonly route: ActivatedRoute,
    private readonly brandService: BrandService, private readonly translate: TranslateService,
    override readonly router: Router,  private readonly ecuService: EcuService,
    private messageService: MessageService, private confirmationService: ConfirmationService,
    override readonly authenticationService: AuthenticationService) {
      super(authenticationService, router);
  }

  ngOnInit() {
    if (!this.allow(this.PROFILE.ADMIN)) {
      this.router.navigate(['/']);
      return;
    }
    this.route.params.subscribe(params => {
      const code:string = params['code'];
      this.brand = null;
      if (!code) {
            // creation form
            this.brand = new Brand('');
      } else {
         this.brandService.findByCode(code).subscribe(b => {
            if (b) {
              this.brand = b;
              this.isCreation = false;
              this.reloadEcu();
            }
            if (this.brand == null) {
              this.brand = new Brand('');
              this.brand.code = code;
            }
         });
        }
    });
  }

  reloadEcu() {
    if (!this.isCreation && this.brand) {
        this.ecuService.findByBrand(this.brand.code).subscribe(ecu => {
          this.ecuList = ecu;
          for(const e of this.ecuList) {
            e.getProcessing();
          }
      });
    }
  }

    
  setEcu(ecu: Ecu): void {
    this.router.navigate([`/edit_ecu/${ecu.brand_code}/${ecu.code}`]);
  }

  deleteEcu(ecu: Ecu) {

    this.confirmationService.confirm({
      message: 'Confirmez-vous la suppression de du calulateur: '+ecu.code+' de la marque: '+ecu.brand_code+' ? Cette opération ne peut pas être annulée.',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.ecuService.delete(ecu.brand_code, ecu.code).subscribe(() =>  this.reloadEcu());
        this.translate.get('WARNING.DATA_DELETED').subscribe(msg => {
          this.messageService.add({ severity: 'info', summary: 'Information', detail: msg });
        });
        },
      reject: () => {
       
      }
  });

  }

  
  
  public cancelForm(): void {
    this.router.navigate(['/brand_list']);
  }
 
  /**
    * Validation of form : save
    */
  public validateForm(): void {
    if (!this.brand || !this.brand.code || !this.brand.name) {
        this.translate.get('WARNING.MISSING_VALUE').subscribe(msg => {
          this.messageService.add({ severity: 'warn', summary: 'Attention', detail: msg });
      });
      return;
    }
    
    if (this.isCreation)
      this.brandService.create(this.brand).subscribe(r => this.success(r));
    else
      this.brandService.set(this.brand).subscribe(r => this.success(r));
  }
 
  success(f: Brand): void {
    this.cancelForm(); // check where we go now
    this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
      this.messageService.add({ severity: 'info', summary: 'Information', detail: msg })
    });
  }
}
