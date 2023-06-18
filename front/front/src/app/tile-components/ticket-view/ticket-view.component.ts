import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';
import {Ticket} from '../../model/ticket';

// services
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';
@Component({
  selector: 'app-ticket-view',
  templateUrl: './ticket-view.component.html',
  styleUrls: ['./ticket-view.component.css']
})
export class TicketViewComponent extends PatchSecured implements OnInit {
  @Input() public ticket: Ticket|null = null;
  @Output() confirmTicketEvent = new EventEmitter<string>();

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router) {
    super(authenticationService, router);
  }
  
  ngOnInit() {
    // NO OP
  }

  validate(resu: boolean): void {
    this.confirmTicketEvent.emit(resu ? "true" : "false");
  }

  getProcessing(): string {
    if (!this.ticket) {
      return '';
    }
    let resu: string[] = [];
    if (this.ticket.dpf) {
      resu.push('DPF');
    }
    if (this.ticket.egr) {
      resu.push('EGR');
    }
    if (this.ticket.lambda) {
      resu.push('Lambda');
    }
    if (this.ticket.hotstart) {
      resu.push('Hotsart');
    }
    if (this.ticket.flap) {
      resu.push('Flap');
    }
    if (this.ticket.adblue) {
      resu.push('Adblue');
    }
    if (this.ticket.dtc) {
      resu.push('DTC');
    }
    if (this.ticket.torqmonitor) {
      resu.push('Torqmonitor');
    }
    if (this.ticket.speedlimit) {
      resu.push('Speedlimit');
    }
    if (this.ticket.startstop) {
      resu.push('Startstop');
    }
    if (this.ticket.nox) {
      resu.push('NOX');
    }
    if (this.ticket.tva) {
      resu.push('TVA');
    }
    if (this.ticket.readiness) {
      resu.push('Readiness');
    }
    if (this.ticket.immo) {
      resu.push('Immo');
    }
    if (this.ticket.maf) {
      resu.push('MAF');
    }
    if (this.ticket.hardcut) {
      resu.push('Hardcut');
    }
    if (this.ticket.displaycalibration) {
      resu.push('Display calibration');
    }
    if (this.ticket.waterpump) {
      resu.push('Waterpump');
    }    
    if (this.ticket.tprot) {
      resu.push('Tprot');
    }    
    if (this.ticket.o2) {
      resu.push('O2');
    }    
    if (this.ticket.glowplugs) {
      resu.push('Glowplugs');
    }    
    if (this.ticket.y75) {
      resu.push('Y75');
    }
    if (this.ticket.special) {
      resu.push('Special');
    }
    if (this.ticket.decata) {
      resu.push('Decata');
    }
    if (this.ticket.vmax) {
      resu.push('Vmax');
    }
    if (this.ticket.stage1) {
      resu.push('Stage1');
    }
    if (this.ticket.stage2) {
      resu.push('Stage2');
    }
    if (this.ticket.flexfuel) {
      resu.push('Flexfuel');
    }

    return resu.join(', ');
  }
}
