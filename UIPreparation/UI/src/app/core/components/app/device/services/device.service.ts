import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Device } from '../models/Device';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  constructor(private httpClient: HttpClient) { }


  getDeviceList(): Observable<Device[]> {

    return this.httpClient.get<Device[]>(environment.getApiUrl + '/devices/getall')
  }

  getDeviceById(id: number): Observable<Device> {
    return this.httpClient.get<Device>(environment.getApiUrl + '/devices/getbyid?id='+id)
  }

  addDevice(device: Device): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/devices/', device, { responseType: 'text' });
  }

  updateDevice(device: Device): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/devices/', device, { responseType: 'text' });

  }

  deleteDevice(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/devices/', { body: { id: id } });
  }


}