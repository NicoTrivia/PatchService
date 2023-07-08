import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { ConfirmationService, MessageService } from 'primeng/api';

import {Ticket} from '../../model/ticket';
import { PatchSecured } from '../../auth/patchSecured';
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
import { TicketService } from '../../services/ticket.service';
import { PROFILE } from 'src/app/auth/profile.enum';

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
    if (this.allow(PROFILE.OPERATOR)) {
      this.ticketService.findAll().subscribe(list => {
        this.ticketList = list;
        for(const t of this.ticketList) {
          this.getProcessing(t);
        }
      });
    } else {
      this.ticketService.findByTenant(this.authenticationService.getTenant()).subscribe(list => {
        this.ticketList = list;
        for(const t of this.ticketList) {
          this.getProcessing(t);
        }
      });
    }
  }

  getProcessing(t: Ticket): string {
    if (t.processing != null) {
      return t.processing;
    }
    let result: string[] = [];
    if (t.dpf) { result.push('dpf');};
    if (t.egr) { result.push('egr');};
    if (t.lambda) { result.push('lambda');};
    if (t.hotstart) { result.push('hotstart');};
    if (t.flap) { result.push('flap');};
    if (t.adblue) { result.push('adblue');};
    if (t.dtc) { result.push('dtc');};
    if (t.torqmonitor) { result.push('torqmonitor');};
    if (t.speedlimit) { result.push('speedlimit');};
    if (t.startstop) { result.push('startstop');};
    if (t.nox) { result.push('nox');};
    if (t.tva) { result.push('tva');};
    if (t.readiness) { result.push('readiness');};
    if (t.immo) { result.push('immo');};
    if (t.maf) { result.push('maf');};
    if (t.hardcut) { result.push('hardcut');};
    if (t.displaycalibration) { result.push('displaycalibration');};
    if (t.waterpump) { result.push('waterpump');};
    if (t.tprot) { result.push('tprot');};
    if (t.o2) { result.push('o2');};
    if (t.glowplugs) { result.push('glowplugs');};
    if (t.y75) { result.push('y75');};
    if (t.special) { result.push('special');};
    if (t.decata) { result.push('decata');};
    if (t.vmax) { result.push('vmax');};
    if (t.stage1) { result.push('stage1');};
    if (t.stage2) { result.push('stage2');};
    if (t.flexfuel) { result.push('flexfuel');};

    let s = '';
    for(const p of result) {
      if (s.length > 0) {
        s = s + ', ';
      }
      s = s + p;
    }
    t.processing = s;
    return s;
  }
}
