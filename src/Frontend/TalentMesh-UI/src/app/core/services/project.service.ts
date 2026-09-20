import { inject, Injectable } from "@angular/core";
import { environment } from "../../environment/environment";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Project } from "../../models/project";
import { CreateProjectRequest } from "../../models/create-project";

@Injectable({
    providedIn : 'root'
})
export class ProjectService{
    
    private http = inject(HttpClient);

    createProject(request : CreateProjectRequest) : Observable<Project> {
        return this.http.post<Project>(`${environment.apiBaseUrl}/project`,request);
    }

    getProjects() : Observable<Project[]> {
        return this.http.get<Project[]>(`${environment.apiBaseUrl}/project`);
                        
    }

    getProjectbyId(id : string) : Observable<Project> {
        return this.http.get<Project>(`${environment.apiBaseUrl}/project/${id}`);
    }

}