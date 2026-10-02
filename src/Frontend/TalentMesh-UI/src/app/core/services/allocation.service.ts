import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Allocation, CreateAllocationRequest } from "../../models/allocation.model";
import { Observable } from "rxjs";
import { environment } from "../../environment/environment";

@Injectable({
    providedIn: 'root'
})
export class AllocationService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/allocation`

    createAllocation(request: CreateAllocationRequest): Observable<Allocation> {
        return this.http.post<Allocation>(this.baseUrl, request);
    }

    getAllocation(id: string): Observable<Allocation> {
        return this.http.get<Allocation>(`${this.baseUrl}/${id}`, {});
    }

    approveAllocation(id: string): Observable<Allocation> {
        return this.http.post<Allocation>(`${this.baseUrl}/${id}/approve`, {});
    }

    rejectAllocation(id: string, reason: string): Observable<Allocation> {
        return this.http.post<Allocation>(`${this.baseUrl}/${id}/reject`, { reason });
    }


    getAllocations(status?: string): Observable<Allocation[]> {
        let params = new HttpParams();

        if (status) {
            params = params.set('status', status);
        }

        return this.http.get<Allocation[]>(this.baseUrl, { params });
    }
}