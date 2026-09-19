import { Component, inject, OnInit } from "@angular/core";
import { Employee } from "../../models/employee";
import { EmployeeService } from "../../core/services/employee.service";

@Component({
    selector: 'app-resource-pool',
    templateUrl: './resource-pool.html',
    styleUrl : 'resource-pool.css'
})
export class Resourcepool implements OnInit {
    loading = false;
    error = '';
    private readonly employeeService = inject(EmployeeService)

    Employees: Employee[] = [];

    ngOnInit(): void {
        this.loadEmployees();
    }

    loadEmployees(): void {
        this.loading = true;
        this.error = '';
        this.employeeService.getAllEmployees().subscribe({
            next: (employees) => {
                this.Employees = employees,
                this.loading = false
            },

            error: (err) => {
                console.log(err);   
                this.error = err;
                this.loading = false;
            }
        });
    }
}