import { Injectable } from '@angular/core';
import {Config} from '../config';
import {HttpClient} from '@angular/common/http';
import {PSCommonService} from './ps-common.service';
import { NGXLogger } from 'ngx-logger';
import {Observable, of} from 'rxjs';
import {User} from '../model/user';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService extends PSCommonService {

  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }

  /**
  * Get the list of available data
  */
  public findAll(): Observable<User[]> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.user}`;
    return this.http.get<Array<User>>(url).pipe(map(resu => {
        const list: User[] = [];

        for (const u of resu) {
            list.push(new User(u));
        }
        return list;
    }));
  }

  
  /**
  * Get
  */
  public findById(id: number): Observable<User|null> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.user}/${id}`;
      return this.http.get<User>(url).pipe(map(resu => {
          if (resu) {
              return new User(resu);
          }
          return null;
      }));
  }
  /**
  * Create
  */
  public create(user: User): Observable<User> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.user}`;
  
    return this.http.put(url, user).pipe(map(p => {
      return new User(p);
    }));
  }

  /**
  * Update
  */
  public set(user: User): Observable<User> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.user}`;

    return this.http.put(url, user).pipe(map(p => {
      return new User(p);
    }));
  }
}
