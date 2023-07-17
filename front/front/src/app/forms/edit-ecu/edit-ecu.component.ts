import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { EcuService } from '../../services/ecu.service';
import { ConfirmationService, MessageService, ConfirmEventType } from 'primeng/api';
import {Config} from '../../config';
import {ItemInterface} from '../../model/ItemInterface';
import {Ecu} from '../../model/ecu';

@Component({
  selector: 'app-edit-ecu',
  templateUrl: './edit-ecu.component.html',
  styleUrls: ['./edit-ecu.component.css']
})
export class EditEcuComponent extends PatchSecured  implements OnInit {
  ecu: Ecu|null = null;
  isCreation = true;
  protected fuelList: ItemInterface[] = [];
  protected fuelSelected: ItemInterface | null = null;
  
  constructor(private readonly route: ActivatedRoute, private readonly translate: TranslateService,
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
      const brand_code:string = params['brand_code'];
      if (!brand_code) {
        this.router.navigate(['/brand_list']);
        return;
      }
      const code:string = params['code'];
      this.ecu = null;
      if (!code) {
            // creation form
            this.ecu = new Ecu('');
            this.ecu.brand_code = brand_code;
      } else {
         this.ecuService.findByBrand(brand_code).subscribe(l => {
            for(const b of l) {
              if (b.code == code) {
                this.ecu = b;
                this.isCreation = false;
                break;
              }
            }
            if (this.ecu == null) {
              this.ecu = new Ecu('');
              this.ecu.brand_code = brand_code;
            }
         });
        }
    });

    this.fuelList =  [
      { name: 'Toutes', code: 'X' },
      { name: 'Diesel', code: 'D' },
      { name: 'Essence', code: 'P' }
    ];
    this.fuelSelected = this.fuelList[0];

  }

  
  public cancelForm(): void {
    if (this.ecu && this.ecu.brand_code) {
      const brand_code = this.ecu.brand_code;
      this.router.navigate([`/edit_brand/${brand_code}`]);
    } else {
      this.router.navigate(['/brand_list']);
    }
  }
 
  /**
    * Validation of form : save
    */
  public validateForm(): void {
    if (!this.ecu || !this.ecu.code || !this.ecu.brand_code) {
        this.translate.get('WARNING.MISSING_VALUE').subscribe(msg => {
          this.messageService.add({ severity: 'warn', summary: 'Attention', detail: msg });
      });
      return;
    }
    if (this.fuelSelected) {
      this.ecu.fuel = this.fuelSelected.code;
    } else {
      this.ecu.fuel = 'X';
    }
    if (this.isCreation)
      this.ecuService.create(this.ecu).subscribe(r => this.success(r));
    else
      this.ecuService.set(this.ecu).subscribe(r => this.success(r));
  }

  success(f: Ecu): void {
    this.cancelForm(); // check where we go now
    this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
      this.messageService.add({ severity: 'info', summary: 'Information', detail: msg })
    });
  }
}
