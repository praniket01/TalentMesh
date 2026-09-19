import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Employee } from "../../models/employee";
import { environment } from "../../environment/environment";

@Injectable({
    providedIn : 'root'
})
export class EmployeeService{

    private http = inject(HttpClient);

    getAllEmployees() : Observable<Employee[]>{
        return this.http.get<Employee[]>(
            `${environment.apiBaseUrl}/employee`
        )
    }
}