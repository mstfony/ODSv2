import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Alert } from './models/Alert';
import { AlertService } from './services/Alert.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-alert',
	templateUrl: './alert.component.html',
	styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name', 'update','delete'];

	alertList:Alert[];
	alert:Alert=new Alert();

	alertAddForm: FormGroup;


	alertId:number;

	constructor(private alertService:AlertService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getAlertList();
    }

	ngOnInit() {

		this.createAlertAddForm();
	}


	getAlertList() {
		this.alertService.getAlertList().subscribe(data => {
			this.alertList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.alertAddForm.valid) {
			this.alert = Object.assign({}, this.alertAddForm.value)

			if (this.alert.id == 0)
				this.addAlert();
			else
				this.updateAlert();
		}

	}

	addAlert(){

		this.alertService.addAlert(this.alert).subscribe(data => {
			this.getAlertList();
			this.alert = new Alert();
			jQuery('#alert').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertAddForm);

		})

	}

	updateAlert(){

		this.alertService.updateAlert(this.alert).subscribe(data => {

			var index=this.alertList.findIndex(x=>x.id==this.alert.id);
			this.alertList[index]=this.alert;
			this.dataSource = new MatTableDataSource(this.alertList);
            this.configDataTable();
			this.alert = new Alert();
			jQuery('#alert').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertAddForm);

		})

	}

	createAlertAddForm() {
		this.alertAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required]
		})
	}

	deleteAlert(alertId:number){
		this.alertService.deleteAlert(alertId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.alertList=this.alertList.filter(x=> x.id!=alertId);
			this.dataSource = new MatTableDataSource(this.alertList);
			this.configDataTable();
		})
	}

	getAlertById(alertId:number){
		this.clearFormGroup(this.alertAddForm);
		this.alertService.getAlertById(alertId).subscribe(data=>{
			this.alert=data;
			this.alertAddForm.patchValue(data);
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
