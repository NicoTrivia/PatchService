import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';
import { saveAs } from 'file-saver';
import { ConfirmationService, MessageService } from 'primeng/api';
import {HttpClient} from '@angular/common/http';
import {Ticket} from '../../model/ticket';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TicketService } from '../../services/ticket.service';
import { PROFILE } from 'src/app/auth/profile.enum';

import {Config} from '../../config';
import {TicketListComponent} from '../ticket-list/ticket-list.component';
@Component({
  selector: 'app-ticket-in-progress',
  templateUrl: '../ticket-list/ticket-list.component.html',
  styleUrls: ['../ticket-list/ticket-list.component.css']
})
export class TicketInProgressComponent extends TicketListComponent {

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, override readonly ticketService: TicketService,
    override readonly translate: TranslateService, override messageService: MessageService,
    override confirmationService: ConfirmationService, override http: HttpClient
    ) {
      super(authenticationService, router, ticketService, translate, messageService, confirmationService, http);
  }
  
  override getTitle(): string {
    return 'IN_PROGRESS_VIEW.TITLE.MAIN';
  }

  override inProgressView(): boolean {
    return true;
  }

  override reload() {
    this.ticketService.findInProgress().subscribe(list => {
        this.ticketList = list;
        for(const t of this.ticketList) {
          this.getProcessing(t);
        }
    });
  }

  override canAckProcessing(t: Ticket): boolean {
    return t && (t.processed_user_id == null || t.processed_user_id == 0);
  }

  override ackProcessing(t: Ticket): void {
    const user = this.authenticationService.getUser();

    if (user && user.id && this.allow(PROFILE.OPERATOR)) {

      // 1 reload ticket to be sure it's still available
      this.ticketService.findById(t.id).subscribe(ticket => {
        if (ticket) {
          if (ticket.processed_user_id == null || ticket.processed_user_id == 0)
          {
            ticket.processed_user_id = user.id;
            ticket.processed_user_name = this.authenticationService.getUserName();
            ticket.processed_date = new Date();
      
            this.ticketService.set(ticket).subscribe(t => {
              this.reload();
              this.translate.get("IN_PROGRESS_VIEW.MSG.PROCESS_TICKET_OK").subscribe(msg => {
                this.messageService.add({ severity: 'success', summary: 'Information', detail: msg });
              });
            });
          } else {
            this.translate.get("IN_PROGRESS_VIEW.MSG.PROCESS_TICKET_ALREADY_TAKEN", {'operator': ticket.processed_user_name}).subscribe(msg => {
              this.messageService.add({ severity: 'warn', summary: 'Attention', detail: msg });
            });
          }
        }
      });
    }
  }

  override uploadPatch(t: Ticket): void {
    // show dialog
    this.currentUploadTicket = t;
    this.uploadDialogVisible = true;
  }

}
