import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorSetting } from '../models/SensorSetting';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SensorSettingService {

  constructor(private httpClient: HttpClient) { }


  getSensorSettingList(): Observable<SensorSetting[]> {

    return this.httpClient.get<SensorSetting[]>(environment.getApiUrl + '/sensorSettings/getall')
  }

  getSensorSettingById(id: number): Observable<SensorSetting> {
    return this.httpClient.get<SensorSetting>(environment.getApiUrl + '/sensorSettings/getbyid?id='+id)
  }

  addSensorSetting(sensorSetting: SensorSetting): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/sensorSettings/', sensorSetting, { responseType: 'text' });
  }

  updateSensorSetting(sensorSetting: SensorSetting): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/sensorSettings/', sensorSetting, { responseType: 'text' });

  }

  deleteSensorSetting(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/sensorSettings/', { body: { id: id } });
  }


}