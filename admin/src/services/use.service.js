import createApiClient from "./api.service.js";
class UseService {
    constructor(baseUrl = "http://localhost:5086/api/v1/uses") {
        this.api = createApiClient(baseUrl);
        this.token = sessionStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async create(data){
        return (await this.api.post("/",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async getOne(id){
        return (await this.api.get(`/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async update(id,data){
        return (await this.api.put(`/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async deleteOne(data){
        return (await this.api.delete(`/${data}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new UseService();