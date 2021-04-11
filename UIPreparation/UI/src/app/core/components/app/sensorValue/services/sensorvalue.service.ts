import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorValue } from '../models/SensorValue';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SensorValueService {

  constructor(private httpClient: HttpClient) { }


  getSensorValueList(): Observable<SensorValue[]> {

    return this.httpClient.get<SensorValue[]>(environment.getApiUrl + '/sensorValues/getall')
  }

  getSensorValueById(id: number): Observable<SensorValue> {
    return this.httpClient.get<SensorValue>(environment.getApiUrl + '/sensorValues/getbyid?id='+id)
  }

  addSensorValue(sensorValue: SensorValue): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/sensorValues/', sensorValue, { responseType: 'text' });
  }

  updateSensorValue(sensorValue: SensorValue): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/sensorValues/', sensorValue, { responseType: 'text' });

  }

  deleteSensorValue(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/sensorValues/', { body: { id: id } });
  }


}