import { Injectable } from '@angular/core';
import {Config} from '../config';
import {HttpClient} from '@angular/common/http';
import {PSCommonService} from './ps-common.service';
import { NGXLogger } from 'ngx-logger';
import {Observable, of} from 'rxjs';
import {Ticket} from '../model/ticket';
import {map} from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class TicketService extends PSCommonService {
  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }


    /**
  * Get the list of available data
  */
    public findAll(): Observable<Ticket[]> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;
      return this.http.get<Array<Ticket>>(url).pipe(map(resu => {
          const list: Ticket[] = [];
  
          for (const u of resu) {
            const tenant = new Ticket(u);
            list.push(tenant);
          }
          return list;
      }));
  }
}
