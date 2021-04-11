import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AlertActionUser } from '../models/AlertActionUser';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AlertActionUserService {

  constructor(private httpClient: HttpClient) { }


  getAlertActionUserList(): Observable<AlertActionUser[]> {

    return this.httpClient.get<AlertActionUser[]>(environment.getApiUrl + '/alertActionUsers/getall')
  }

  getAlertActionUserById(id: number): Observable<AlertActionUser> {
    return this.httpClient.get<AlertActionUser>(environment.getApiUrl + '/alertActionUsers/getbyid?id='+id)
  }

  addAlertActionUser(alertActionUser: AlertActionUser): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/alertActionUsers/', alertActionUser, { responseType: 'text' });
  }

  updateAlertActionUser(alertActionUser: AlertActionUser): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/alertActionUsers/', alertActionUser, { responseType: 'text' });

  }

  deleteAlertActionUser(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/alertActionUsers/', { body: { id: id } });
  }


}