import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Device } from './models/Device';
import { DeviceService } from './services/Device.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-device',
	templateUrl: './device.component.html',
	styleUrls: ['./device.component.scss']
})
export class DeviceComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name','location','describe', 'update','delete'];

	deviceList:Device[];
	device:Device=new Device();

	deviceAddForm: FormGroup;


	deviceId:number;

	constructor(private deviceService:DeviceService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getDeviceList();
    }

	ngOnInit() {

		this.createDeviceAddForm();
	}


	getDeviceList() {
		this.deviceService.getDeviceList().subscribe(data => {
			this.deviceList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.deviceAddForm.valid) {
			this.device = Object.assign({}, this.deviceAddForm.value)

			if (this.device.id == 0)
				this.addDevice();
			else
				this.updateDevice();
		}

	}

	addDevice(){

		this.deviceService.addDevice(this.device).subscribe(data => {
			this.getDeviceList();
			this.device = new Device();
			jQuery('#device').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.deviceAddForm);

		})

	}

	updateDevice(){

		this.deviceService.updateDevice(this.device).subscribe(data => {

			var index=this.deviceList.findIndex(x=>x.id==this.device.id);
			this.deviceList[index]=this.device;
			this.dataSource = new MatTableDataSource(this.deviceList);
            this.configDataTable();
			this.device = new Device();
			jQuery('#device').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.deviceAddForm);

		})

	}

	createDeviceAddForm() {
		this.deviceAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
location : ["", Validators.required],
describe : ["", Validators.required]
		})
	}

	deleteDevice(deviceId:number){
		this.deviceService.deleteDevice(deviceId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.deviceList=this.deviceList.filter(x=> x.id!=deviceId);
			this.dataSource = new MatTableDataSource(this.deviceList);
			this.configDataTable();
		})
	}

	getDeviceById(deviceId:number){
		this.clearFormGroup(this.deviceAddForm);
		this.deviceService.getDeviceById(deviceId).subscribe(data=>{
			this.device=data;
			this.deviceAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
	}

	configDataTable(): void {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

  }
