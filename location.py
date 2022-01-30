from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
import time
import os

path = os.getcwd()

option = webdriver.ChromeOptions()
option.add_argument('headless')
service = Service(executable_path='{}/chromedriver'.format(path))
browser = webdriver.Chrome(service=service, options=option)

def get_location():

    #Truy cập trang web https://www.google.com/maps để lấy tạo độ hiện tại
    browser.get('https://www.google.com/maps')

    #Chờ 5s để load trang googlemap
    time.sleep(5)
    #Tìm đến button mylocation và thực hiện click
    browser.find_element(By.ID, 'pWhrzc-mylocation').click()
    time.sleep(5)

    #Lấy url hiện tại để lấy kinh độ và vĩ độ.
    url = browser.current_url
    latitude = float(url.split(',')[0].split('@')[-1])
    longitude = float(url.split(',')[1])
    return latitude, longitude


