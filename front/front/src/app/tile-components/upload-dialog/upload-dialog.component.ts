import { Component, SimpleChanges, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

import { MessageService } from 'primeng/api';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';
import {Ticket} from '../../model/ticket';

// services
import { AuthenticationService } from '../../auth/authentication-service/authentication-service';

@Component({
  selector: 'app-upload-dialog',
  templateUrl: './upload-dialog.component.html',
  styleUrls: ['./upload-dialog.component.css']
})
export class UploadDialogComponent extends PatchSecured implements OnInit, OnChanges {
  @Input() ticket: Ticket|null = null;
  @Output() confirmUpload = new EventEmitter<string>();
  MAX_CHARS_COMMENT = 250;

  protected fileName: string|null = null;
  protected fileSize: number = 0;
  protected failedPatch: boolean = false;

  constructor(override readonly authenticationService: AuthenticationService,
    override readonly router: Router, private readonly translate: TranslateService,
    private messageService: MessageService) {
    super(authenticationService, router);
  }

  ngOnInit() {
    this.fileName = null;
    this.fileSize = 0;
    this.failedPatch = false;
  }

  ngOnChanges(changes: SimpleChanges) {
      if ('ticket' in changes) {
        this.ngOnInit();
      }
  }

  validate(resu: boolean): void {
    if (resu && this.ticket) {
      if (this.failedPatch) {
        this.ticket.processed_file_name = 'failed';
        this.ticket.processed_file_size = 0;
      } else {
        this.ticket.processed_file_name = this.fileName;
        this.ticket.processed_file_size = this.fileSize;
      }
      this.ticket.processed_user_id = this.authenticationService.getUser()!.id;
      this.ticket.processed_user_name = this.authenticationService.getUserName();
      this.ticket.processed_date = new Date();
    }
    this.confirmUpload.emit(resu ? "true" : "false");
  }
  
  onBasicUploadAuto(event: any) {
    if (event.files && event.files[0])
    {
      this.fileName = event.files[0].name;
      this.fileSize = event.files[0].size;
      if (this.fileSize > 0) {
        this.fileSize = Math.trunc(this.fileSize / 1024);
      }
      
      this.translate.get("IN_PROGRESS_VIEW.MSG.UPLOAD_PATCH_OK").subscribe(msg => {
        this.messageService.add({ severity: 'info', summary: 'Information', detail: msg });
      });
    }
  }
  
  getUploadUrl(): string {
    const id = this.ticket?.file_id;
    const url = `${Config.APP_URL}${Config.API_ROUTES.files_patched}/${id}`;
    return url;
  }
}
