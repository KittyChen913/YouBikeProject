<template>
  <div class="Index">
    <a-layout-content :style="{ padding: '1px 50px', marginTop: '64px' }">
      <div :style="{ background: '#fff',marginTop: '64px', padding: '24px', minHeight: '370px' }">
        <h2>Youbike 站點歷史資料查詢</h2>

        <a-form layout="inline">
          <a-form-item label="場站區域">
            <a-select v-model="regionName" style="width: 120px">
              <a-select-option value="">all</a-select-option>
              <a-select-option
                v-for="region in RegionList"
                :key="region.areaname"
              >
                {{ region.areaname }}
              </a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item label="場站名稱">
            <a-select v-model="stationId" style="width: 250px">
              <a-select-option value="">all</a-select-option>
              <a-select-option
                v-for="station in YouBikeStationList"
                :key="station.sno"
              >
                {{ station.sna }}
              </a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item>
            <a-button type="primary" @click="GetYoubikeLogList">查詢</a-button>
          </a-form-item>
        </a-form>

        <div>
          <a-table
            ref="table"
            :columns="Tablecolumns"
            :dataSource="YoubikeLogList"
            rowKey="logid"
          >
            <a slot="name" slot-scope="text">{{ text }}</a>
          </a-table>
        </div>
      </div>
    </a-layout-content>
    <a-layout-footer :style="{ textAlign: 'center' }">
      Ant Design 2021 Created by Kitty Chen
      <div>
        Icons made by
        <a href="https://www.freepik.com" title="Freepik">Freepik</a> from
        <a href="https://www.flaticon.com/" title="Flaticon"
          >www.flaticon.com</a
        >
      </div>
    </a-layout-footer>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import 'ant-design-vue/dist/antd.css'
import Axios from 'axios'

export default Vue.extend({
  mounted() {
    this.init()
  },
  data() {
    return {
      RegionList: [],
      YouBikeStationList: [],
      YoubikeLogList: [],
      regionName: '',
      stationId: '',
      Tablecolumns: [
        {
          title: '站點代號',
          dataIndex: 'sno',
          key: 'sno',
        },
        {
          title: '場站名稱',
          dataIndex: 'sna',
          key: 'sna',
        },
        {
          title: '場站區域',
          dataIndex: 'sarea',
          key: 'sarea',
        },
        {
          title: '目前車輛數量',
          dataIndex: 'sbi',
          key: 'sbi',
        },
        {
          title: '空位數量',
          dataIndex: 'bemp',
          key: 'bemp',
        },
        {
          title: '官方更新時間',
          dataIndex: 'mday',
          key: 'mday',
        },
        {
          title: 'Log 紀錄時間',
          dataIndex: 'updatedatetime',
          key: 'updatedatetime',
        },
      ]
    }
  },
  methods: {
    init() {
      this.GetRegionList(), this.GetYouBikeStationList()
    },
    GetRegionList() {
      Axios.post('https://localhost:5001/api/Youbike/GetRegionList').then(
        (response) => {
          this.RegionList = response.data
        }
      )
    },
    GetYouBikeStationList() {
      Axios.post(
        'https://localhost:5001/api/Youbike/GetYouBikeStationList'
      ).then((response) => {
        this.YouBikeStationList = response.data
      })
    },
    GetYoubikeLogList() {
      var params = { Sno: this.stationId, Sarea: this.regionName }
      Axios.post('https://localhost:5001/api/Youbike/GetYoubikeLogList', params)
        .then((response) => {
          this.YoubikeLogList = response.data
        })
    },
  },
})
</script>
