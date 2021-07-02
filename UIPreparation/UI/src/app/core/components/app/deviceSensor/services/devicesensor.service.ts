import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DeviceSensor } from '../models/DeviceSensor';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class DeviceSensorService {

  constructor(private httpClient: HttpClient) { }


  getDeviceSensorList(): Observable<DeviceSensor[]> {

    return this.httpClient.get<DeviceSensor[]>(environment.getApiUrl + '/deviceSensors/getall')
  }

  getDeviceSensorById(id: number): Observable<DeviceSensor> {
    return this.httpClient.get<DeviceSensor>(environment.getApiUrl + '/deviceSensors/getbyid?id='+id)
  }

  addDeviceSensor(deviceSensor: DeviceSensor): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/deviceSensors/', deviceSensor, { responseType: 'text' });
  }

  updateDeviceSensor(deviceSensor: DeviceSensor): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/deviceSensors/', deviceSensor, { responseType: 'text' });

  }

  deleteDeviceSensor(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/deviceSensors/', { body: { id: id } });
  }


}