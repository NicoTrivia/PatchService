import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { MessageService } from 'primeng/api';

import {User} from '../../model/user';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends PatchSecured implements OnInit {
  public userList:User[] = [];

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly userService: UserService,
    private readonly translate: TranslateService, private messageService: MessageService
    ) {
    super(authenticationService, router);
  }
  
  ngOnInit() {
    this.reload();
  }

  reload() {
    // load list from server
    this.userService.findAll().subscribe(list => {
      this.userList = list;
    });
  }

  public activate(id: number, status: boolean): void {
    if (this.authenticationService.getUser()?.id === id) {
        this.translate.get('WARNING.NOT_SELF').subscribe(msg => {
            this.messageService.add({ severity: 'warn', summary: 'Information', detail: msg });
        });
        return;
    }
    this.userService.findById(id).subscribe(u => {
        if (u != null) {
            u.active = status;
            this.userService.set(u).subscribe(s =>  this.reload());
            this.translate.get('WARNING.DATA_SAVED').subscribe(msg => {
              this.messageService.add({ severity: 'info', summary: 'Information', detail: msg });
            });
        }
    });
  }

  public set(id: number): void {
    this.router.navigate([`/user/${id}`]);
  }

  public setPassword(id: number): void {
      this.router.navigate([`/password/${id}`]);
  }
}
