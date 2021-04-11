import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { SensorLocation } from './models/SensorLocation';
import { SensorLocationService } from './services/SensorLocation.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-sensorLocation',
	templateUrl: './sensorLocation.component.html',
	styleUrls: ['./sensorLocation.component.scss']
})
export class SensorLocationComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','sensorId','locationId', 'update','delete'];

	sensorLocationList:SensorLocation[];
	sensorLocation:SensorLocation=new SensorLocation();

	sensorLocationAddForm: FormGroup;


	sensorLocationId:number;

	constructor(private sensorLocationService:SensorLocationService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getSensorLocationList();
    }

	ngOnInit() {

		this.createSensorLocationAddForm();
	}


	getSensorLocationList() {
		this.sensorLocationService.getSensorLocationList().subscribe(data => {
			this.sensorLocationList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.sensorLocationAddForm.valid) {
			this.sensorLocation = Object.assign({}, this.sensorLocationAddForm.value)

			if (this.sensorLocation.id == 0)
				this.addSensorLocation();
			else
				this.updateSensorLocation();
		}

	}

	addSensorLocation(){

		this.sensorLocationService.addSensorLocation(this.sensorLocation).subscribe(data => {
			this.getSensorLocationList();
			this.sensorLocation = new SensorLocation();
			jQuery('#sensorlocation').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorLocationAddForm);

		})

	}

	updateSensorLocation(){

		this.sensorLocationService.updateSensorLocation(this.sensorLocation).subscribe(data => {

			var index=this.sensorLocationList.findIndex(x=>x.id==this.sensorLocation.id);
			this.sensorLocationList[index]=this.sensorLocation;
			this.dataSource = new MatTableDataSource(this.sensorLocationList);
            this.configDataTable();
			this.sensorLocation = new SensorLocation();
			jQuery('#sensorlocation').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorLocationAddForm);

		})

	}

	createSensorLocationAddForm() {
		this.sensorLocationAddForm = this.formBuilder.group({		
			id : [0],
sensorId : [0, Validators.required],
locationId : [0, Validators.required]
		})
	}

	deleteSensorLocation(sensorLocationId:number){
		this.sensorLocationService.deleteSensorLocation(sensorLocationId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.sensorLocationList=this.sensorLocationList.filter(x=> x.id!=sensorLocationId);
			this.dataSource = new MatTableDataSource(this.sensorLocationList);
			this.configDataTable();
		})
	}

	getSensorLocationById(sensorLocationId:number){
		this.clearFormGroup(this.sensorLocationAddForm);
		this.sensorLocationService.getSensorLocationById(sensorLocationId).subscribe(data=>{
			this.sensorLocation=data;
			this.sensorLocationAddForm.patchValue(data);
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
