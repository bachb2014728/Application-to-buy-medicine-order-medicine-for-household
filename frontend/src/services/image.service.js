import createApiClient from "./api.service.js";
class ImageClientService {
    constructor(baseUrl = "http://localhost:5086/api/v1/images") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async uploadImage(data) {
        return (await this.api.post("/", data,{
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
    async getOne(id){
        return (await this.api.get(`${id}`,{
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
    async delete(id){
        return (await this.api.delete(`${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`,
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
}
export default new ImageClientService();