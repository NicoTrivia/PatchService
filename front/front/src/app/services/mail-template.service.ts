import { Injectable } from '@angular/core';
import {PSCommonService} from './ps-common.service';
import {HttpClient} from '@angular/common/http';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import { NGXLogger } from 'ngx-logger';
import {Config} from '../config';
import {MailTemplate} from '../model/mailTemplate';

@Injectable({
  providedIn: 'root'
})
export class MailTemplateService extends PSCommonService {

  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }

  /**
  * Get data
  */
   public findMailTemplate(): Observable<MailTemplate|null> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.mail_template}`;
    return this.http.get<MailTemplate>(url).pipe(map(resu => {
        let mailTemplate: MailTemplate|null = null;
        if (resu != null) {
          mailTemplate = new MailTemplate(resu);
        }
        return mailTemplate;
    }));
  }

  /**
  * Add
  */
  public create(mailTemplate: MailTemplate): Observable<MailTemplate> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.mail_template}`;

    return this.http.post(url, mailTemplate).pipe(map(p => {
      return new MailTemplate(p);
    }));
  }


  /**
  * Update
  */
  public set(mailTemplate: MailTemplate): Observable<MailTemplate> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.mail_template}`;

    return this.http.put(url, mailTemplate).pipe(map(p => {
      return new MailTemplate(p);
    }));
  }
}
