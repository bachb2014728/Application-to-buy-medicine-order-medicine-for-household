import createApiClient from "./api.service.js";
class CustomerClientService {
    constructor(baseUrl = "http://localhost:5086/api/v1/customers") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/",{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async update(data,id){
        return (await this.api.put(`/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async create(data){
        return (await this.api.post("/",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async profile(){
        return (await this.api.get("/profile",{
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
}
export default new CustomerClientService();