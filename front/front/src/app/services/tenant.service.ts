import { Injectable } from '@angular/core';
import {Config} from '../config';
import {HttpClient} from '@angular/common/http';
import {PSCommonService} from './ps-common.service';
import { NGXLogger } from 'ngx-logger';
import {Observable, of} from 'rxjs';
import {Tenant} from '../model/tenant';
import {map} from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class TenantService extends PSCommonService {

  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }

    /**
  * Get the list of available data
  */
    public findAll(): Observable<Tenant[]> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;
      return this.http.get<Array<Tenant>>(url).pipe(map(resu => {
          const list: Tenant[] = [];
  
          for (const u of resu) {
              list.push(new Tenant(u));
          }
          return list;
      }));
    }
  
    /**
  * Get
  */
    public findByCode(code: string): Observable<Tenant|null> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}/${code}`;
      return this.http.get<Tenant>(url).pipe(map(resu => {
          if (resu) {
              return new Tenant(resu);
          }
          return null;
      }));
  }

  /**
  * Update
  */
  public create(user: Tenant): Observable<Tenant> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;
  
      return this.http.post(url, user).pipe(map(p => {
        return new Tenant(p);
      }));
  }
  

  /**
  * Update
  */
  public set(user: Tenant): Observable<Tenant> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;

    return this.http.put(url, user).pipe(map(p => {
      return new Tenant(p);
    }));
  }
}
