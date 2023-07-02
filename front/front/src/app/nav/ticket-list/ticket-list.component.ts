import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { ConfirmationService, MessageService } from 'primeng/api';

import {Ticket} from '../../model/ticket';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TicketService } from '../../services/ticket.service';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent extends PatchSecured implements OnInit {
  
  public ticketList:Ticket[] = [];

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly ticketService: TicketService,
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
    this.ticketService.findAll().subscribe(list => {
      this.ticketList = list;
    });
  }
}
