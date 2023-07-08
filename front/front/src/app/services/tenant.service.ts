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
  public findAll(activeOnly: boolean): Observable<Tenant[]> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;
      return this.http.get<Array<Tenant>>(url).pipe(map(resu => {
          const list: Tenant[] = [];
  
          for (const u of resu) {
            const tenant = new Tenant(u);
            if (!activeOnly || tenant.active)
              list.push(tenant);
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
  public create(tenant: Tenant): Observable<Tenant> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;
  
      return this.http.post(url, tenant).pipe(map(p => {
        return new Tenant(p);
      }));
  }
  

  /**
  * Update
  */
  public set(tenant: Tenant): Observable<Tenant> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}`;

    return this.http.put(url, tenant).pipe(map(p => {
      return new Tenant(p);
    }));
  }

 /**
  * delete
  */
    public delete(code: string): Observable<boolean> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.tenant}/${code}`;
  
      return this.http.delete<boolean>(url).pipe(map(p => {
        return true;
      }));
  }

  public getNextFileId(): Observable<number> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.next_file_id}`;
    return this.http.get<number>(url).pipe(map(resu => {
      if (resu) {
          return resu;
      }
      return 0;
    }));
  }
}
