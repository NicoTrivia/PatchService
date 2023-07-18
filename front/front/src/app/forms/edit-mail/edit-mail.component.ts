import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { MailTemplateService } from '../../services/mail-template.service';
import { MessageService} from 'primeng/api';
import {Config} from '../../config';

import {MailTemplate} from '../../model/mailTemplate';

@Component({
  selector: 'app-edit-mail',
  templateUrl: './edit-mail.component.html',
  styleUrls: ['./edit-mail.component.css']
})
export class EditMailComponent extends PatchSecured  implements OnInit {

  mailTemplate: MailTemplate|null = null;
  isCreation = true;

  constructor(private readonly route: ActivatedRoute,
    private readonly translate: TranslateService, private messageService: MessageService, 
    override readonly router: Router,  private readonly mailTemplateService: MailTemplateService,
    override readonly authenticationService: AuthenticationService) {
      super(authenticationService, router);
  }

  ngOnInit() {
    if (!this.allow(this.PROFILE.ADMIN)) {
      this.router.navigate(['/']);
      return;
    }
    this.isCreation = true;
    this.mailTemplateService.findMailTemplate().subscribe(b => {
            if (b) {
              this.mailTemplate = b;
              this.isCreation = false;
            }
            if (this.mailTemplate == null) {
              this.mailTemplate = new MailTemplate('');
            }
    });
  }
  
  public cancelForm(): void {
    this.router.navigate(['/user_list']);
  }
 
  /**
    * Validation of form : save
    */
  public validateForm(): void {
    if (!this.mailTemplate || !this.mailTemplate.mailAcknowledge || !this.mailTemplate.mailCompleted) {
        this.translate.get('WARNING.MISSING_VALUE').subscribe(msg => {
          this.messageService.add({ severity: 'warn', summary: 'Attention', detail: msg });
      });
      return;
    }
    
    if (this.isCreation)
      this.mailTemplateService.create(this.mailTemplate).subscribe(r => this.success(r));
    else
      this.mailTemplateService.set(this.mailTemplate).subscribe(r => this.success(r));
  }
 
  success(f: MailTemplate): void {
    this.cancelForm(); // check where we go now
    this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
      this.messageService.add({ severity: 'info', summary: 'Information', detail: msg })
    });
  }
}
