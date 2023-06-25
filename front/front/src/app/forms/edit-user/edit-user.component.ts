import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TranslateService} from '@ngx-translate/core';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import {UserService} from '../../services/user.service';
import { TenantService } from '../../services/tenant.service';
import { MessageService } from 'primeng/api';
import {Config} from '../../config';

// data
import {User} from '../../model/user';
@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent extends PatchSecured  implements OnInit {
      private editPassword: boolean = false;
      user: User|null = null;
      private password: string|null = null;
      private password2: string|null = null;
      constructor(private readonly route: ActivatedRoute, private readonly tenantService: TenantService,
          private readonly userService: UserService, private readonly translate: TranslateService,
          override readonly router: Router,
          private messageService: MessageService,
          override readonly authenticationService: AuthenticationService) {
          super(authenticationService, router);
      }
  
      ngOnInit() {
          this.route.params.subscribe(params => {
              const cId:string = params['id'];
              console.log("cId", cId);
              if (!cId || parseInt(cId, 10) <= 0) {
                  // creation form
                  this.user = new User('');
                  this.user.id = -1;
                  this.user.active = true;
              } else {
                  const cPassword = params['password'];
                  if (cPassword) {
                      this.editPassword = true;
                  } else {
                      this.editPassword = false;
                  }
                  // set form : get value
                  this.userService.findById(parseInt(cId, 10)).subscribe((user) => {
                      if (user) {
                          this.user = user;
                      } else {
                        this.user = new User('');
                        this.user.id = -1;
                        this.user.active = true;
                      }
                  });
              }
          });
      }
  
      /**
       * Validation of form : save
       */
      public validateForm(): void {
          if (!this.user || !this.user.login || !this.user.lastname) {
              this.translate.get('WARNING.NO_VALUE').subscribe(msg => {
                this.messageService.add({ severity: 'warning', summary: 'Attention', detail: msg });
          });

              
              return;
          }
          if (this.editPassword) {
              if (this.password && this.password2) {
                  const strongRegex = new RegExp('^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})');
                  if (this.password.length < 10)  {
                      this.translate.get('USER.PASSWORD.MSG.PASSWORD_TOO_SHORT').subscribe(msg =>  this.messageService.add({ severity: 'warning', summary: 'Attention', detail: msg }));
                      return;
                  } else if (!strongRegex.test(this.password))  {
                      this.translate.get('USER.PASSWORD.MSG.PASSWORD_STRENGTH').subscribe(msg =>  this.messageService.add({ severity: 'warning', summary: 'Attention', detail: msg }));
                      return;
                  } else if (this.password !== this.password2) {
                      this.translate.get('USER.PASSWORD.MSG.PASSWORD_MISMATCH').subscribe(msg =>  this.messageService.add({ severity: 'warning', summary: 'Attention', detail: msg }));
                      return;
                  }
              } else {
                  this.translate.get('USER.PASSWORD.MSG.PASSWORD_EMPTY').subscribe(msg =>  this.messageService.add({ severity: 'warning', summary: 'Attention', detail: msg }));
                  return;
              }
          }
       
          if (this.editPassword) {
             // user.setPassword(this.password);
          }
         else if (this.user.id>0)
         {
            this.userService.set(this.user).subscribe(r => this.success(r));
         }
         else
         {
            this.userService.create(this.user).subscribe(r => this.success(r));
         }
      }
  
      public cancelForm(): void {
        this.router.navigate(['/user/list']);
      }
  
      success(f: User): void {
          this.cancelForm(); // check where we go now
          this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
            this.messageService.add({ severity: 'info', summary: 'Informarion', detail: msg })
          });
      }
    
      public isEditPasword(): boolean {
          return this.editPassword;
      }
  
      get Password(): string|null {
          return this.password;
      }
  
      set Password(p: string|null) {
          this.password = p;
      }
  
      get Password2(): string|null {
          return this.password2;
      }
  
      set Password2(p: string|null) {
          this.password2 = p;
      }
  }
  