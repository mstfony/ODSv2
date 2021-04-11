import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AlertActionLog } from '../models/AlertActionLog';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AlertActionLogService {

  constructor(private httpClient: HttpClient) { }


  getAlertActionLogList(): Observable<AlertActionLog[]> {

    return this.httpClient.get<AlertActionLog[]>(environment.getApiUrl + '/alertActionLogs/getall')
  }

  getAlertActionLogById(id: number): Observable<AlertActionLog> {
    return this.httpClient.get<AlertActionLog>(environment.getApiUrl + '/alertActionLogs/getbyid?id='+id)
  }

  addAlertActionLog(alertActionLog: AlertActionLog): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/alertActionLogs/', alertActionLog, { responseType: 'text' });
  }

  updateAlertActionLog(alertActionLog: AlertActionLog): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/alertActionLogs/', alertActionLog, { responseType: 'text' });

  }

  deleteAlertActionLog(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/alertActionLogs/', { body: { id: id } });
  }


}