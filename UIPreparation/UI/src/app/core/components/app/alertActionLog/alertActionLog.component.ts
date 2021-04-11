import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { AlertActionLog } from './models/AlertActionLog';
import { AlertActionLogService } from './services/AlertActionLog.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-alertActionLog',
	templateUrl: './alertActionLog.component.html',
	styleUrls: ['./alertActionLog.component.scss']
})
export class AlertActionLogComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','alertActionUserId','dateTime', 'update','delete'];

	alertActionLogList:AlertActionLog[];
	alertActionLog:AlertActionLog=new AlertActionLog();

	alertActionLogAddForm: FormGroup;


	alertActionLogId:number;

	constructor(private alertActionLogService:AlertActionLogService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getAlertActionLogList();
    }

	ngOnInit() {

		this.createAlertActionLogAddForm();
	}


	getAlertActionLogList() {
		this.alertActionLogService.getAlertActionLogList().subscribe(data => {
			this.alertActionLogList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.alertActionLogAddForm.valid) {
			this.alertActionLog = Object.assign({}, this.alertActionLogAddForm.value)

			if (this.alertActionLog.id == 0)
				this.addAlertActionLog();
			else
				this.updateAlertActionLog();
		}

	}

	addAlertActionLog(){

		this.alertActionLogService.addAlertActionLog(this.alertActionLog).subscribe(data => {
			this.getAlertActionLogList();
			this.alertActionLog = new AlertActionLog();
			jQuery('#alertactionlog').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertActionLogAddForm);

		})

	}

	updateAlertActionLog(){

		this.alertActionLogService.updateAlertActionLog(this.alertActionLog).subscribe(data => {

			var index=this.alertActionLogList.findIndex(x=>x.id==this.alertActionLog.id);
			this.alertActionLogList[index]=this.alertActionLog;
			this.dataSource = new MatTableDataSource(this.alertActionLogList);
            this.configDataTable();
			this.alertActionLog = new AlertActionLog();
			jQuery('#alertactionlog').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertActionLogAddForm);

		})

	}

	createAlertActionLogAddForm() {
		this.alertActionLogAddForm = this.formBuilder.group({		
			id : [0],
alertActionUserId : [0, Validators.required],
dateTime : [null, Validators.required]
		})
	}

	deleteAlertActionLog(alertActionLogId:number){
		this.alertActionLogService.deleteAlertActionLog(alertActionLogId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.alertActionLogList=this.alertActionLogList.filter(x=> x.id!=alertActionLogId);
			this.dataSource = new MatTableDataSource(this.alertActionLogList);
			this.configDataTable();
		})
	}

	getAlertActionLogById(alertActionLogId:number){
		this.clearFormGroup(this.alertActionLogAddForm);
		this.alertActionLogService.getAlertActionLogById(alertActionLogId).subscribe(data=>{
			this.alertActionLog=data;
			this.alertActionLogAddForm.patchValue(data);
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
