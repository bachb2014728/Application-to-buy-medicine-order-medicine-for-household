import createApiClient from "./api.service.js";
class ReceiverService {
    constructor(baseUrl = "http://localhost:5086/api/v1/receivers") {
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
    async create(data){
        return (await this.api.post("/",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async checked(data){
        return (await this.api.put("/checked",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async update(id,data){
        return (await this.api.put(`/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async deleteOne(id){
        return (await this.api.delete(`/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new ReceiverService();