import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';
import { Router} from '@angular/router';

import {Config} from '../../config';
import { PatchSecured } from '../../auth/patchSecured';

@Component({
  selector: 'app-request-patch',
  templateUrl: './request-patch.component.html',
  styleUrls: ['./request-patch.component.css']
})
export class RequestPatchComponent extends PatchSecured {
  
  constructor(private readonly translate: TranslateService, private messageService: MessageService, 
    override readonly router: Router) {
    super(router);
  }

  onBasicUploadAuto(event: any) {
    this.messageService.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded with Auto Mode' });
}
}
