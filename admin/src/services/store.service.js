import createApiClient from "./api.service.js";
class StoreService {
    constructor(baseUrl = "http://localhost:5086/api/v1/store") {
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
    async getOneByURL(url){
        return (await this.api.get(`URL/${url}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async getMyStore(){
        return (await this.api.get("/my-store",{
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
    async updateByURL(url,data){
        return (await this.api.put(`URL/${url}/info`,data,{
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
    async changeAvatar(data,url){
        return (await this.api.put(`URL/${url}/avatar`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async changeBackground(data,url){
        return (await this.api.put(`URL/${url}/background`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
}
export default new StoreService();